using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using ToolManage.Models;
using System.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.Ajax.Utilities;

namespace ToolManage.Helper
{
    public class Detect
    {
        public Detect()
        {

        }

        public void SendMail()
        {
            var db = new ToolManageDataContext();
            var workcells = db.WorkCell.Where(i => i.State == "0" && i.ContactEmail != null).ToList();
            workcells.ForEach(workcell => new DetectHelper(workcell.Id).SendEmail());
            db.Dispose();
        }
    }

    public class DetectHelper
    {
        private readonly ToolManageDataContext db = new ToolManageDataContext();
        private readonly double sendEmailChance = double.Parse(ConfigurationManager.AppSettings["SendEmailChance"]);
        private readonly int sendEmailInterval = int.Parse(ConfigurationManager.AppSettings["SendEmailInterval"]);
        private readonly int col = 3;
        private WorkCell workCell;
        private int row;

        private double[,] matrix, X, Y, XT, XTX, XTXInv, XTXInvXT, B;

        public DetectHelper(int workcellId)
        {
            workCell = db.WorkCell.Find(workcellId);
            if (workCell == null)
            {
                throw new Exception("部门未找到");
            }
        }

        public void SendEmail()
        {
            var value = Calculate();
            var codes = new List<string>();
            foreach (var toolEntity in db.ToolEntity.Where(i => i.ToolDef.WorkCellId == workCell.Id && i.State != "9" && i.State != "3"))
            {
                if (value[0] + value[1] * toolEntity.UsedCount + value[2] * db.RepairApplication.Where(i => i.ToolEntityId == toolEntity.Id && i.State != "2").Count()
                    >= double.Parse(ConfigurationManager.AppSettings["SendEmailChance"]))
                {
                    codes.Add(toolEntity.Code);
                }
            }

            var msg = new MailMessage
            {
                From = new MailAddress(ConfigurationManager.AppSettings["SMTPAccount"], ConfigurationManager.AppSettings["SendMailName"]),
                Subject = "工夹具报废预测",
            };
            codes.ForEach(i =>
            {
                msg.Body += i + " 经系统预测，可能到达报废标准，请留意.\n";
            });
            var client = new SmtpClient()
            {
                EnableSsl = bool.Parse(ConfigurationManager.AppSettings["SMTPEnableSsl"]),
                UseDefaultCredentials = bool.Parse(ConfigurationManager.AppSettings["SMTPUseDefaultCredentials"]),
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SMTPAccount"], ConfigurationManager.AppSettings["SMTPPassword"]),
                Host = "smtp.qq.com",
            };
            try
            {
                client.Send(msg);
            }
            catch (Exception)
            {
            }
        }

        private double[] Calculate()
        {
            row = db.ToolEntity.Where(i => i.ToolDef.WorkCellId == workCell.Id).Count();
            matrix = new double[row, col];
            X = new double[row, col];
            Y = new double[row, 1];
            XT = new double[col, row];
            XTX = new double[col, 2 * col];
            XTXInv = new double[col, col];
            XTXInvXT = new double[col, row];
            B = new double[col, 1];
            int index = 0;
            foreach (var toolEntity in db.ToolEntity.Where(i => i.ToolDef.WorkCellId == workCell.Id && i.State != "9"))
            {
                matrix[index, 0] = toolEntity.UsedCount;
                matrix[index, 1] = db.RepairApplication.Where(i => i.ToolEntityId == toolEntity.Id && i.State != "2").Count();
                matrix[index, 2] = toolEntity.State == "3" ? 1 : 0;
                index++;
            }

            for (int i = 0; i < row; i++)
            { //提取1X和Y数组列
                X[i, 0] = 1;
                Y[i, 0] = matrix[i, col - 1];
                for (int j = 0; j < col - 1; j++)
                    X[i, j + 1] = matrix[i, j];
            }

            Transpose(X, XT, row, col);
            Multipl(XT, X, XTX, col, row, col);
            Inver(XTX, XTXInv, col);
            Multipl(XTXInv, XT, XTXInvXT, col, col, row);
            Multipl(XTXInvXT, Y, B, col, row, 1);

            var value = new double[col];
            for (int i = 0; i < col; i++)
            {
                value[i] = B[i, 0];
            }

            return value;
        }

        private void Transpose(double[,] p1, double[,] p2, int m, int n)
        { //矩阵转置
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    p2[i, j] = p1[j, i];
                }
            }
        }

        private void Multipl(double[,] p1, double[,] p2, double[,] p3, int m, int n, int p)
        { //矩阵相乘
            double sum;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < p; j++)
                {
                    sum = 0;
                    for (int k = 0; k < n; k++)
                        sum += p1[i, k] * p2[k, j];
                    p3[i, j] = sum;
                }
            }
        }

        private void Inver(double[,] p1, double[,] p2, int n)
        { //求逆矩阵

            //初始化矩阵在右侧加入单位阵
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    p1[i, j + n] = 0;
                    p1[i, i + n] = 1;
                }
            }

            //对于对角元素为0的进行换行操作
            for (int i = 0; i < n; i++)
            {
                while (p1[i, i] == 0)
                {
                    for (int j = i + 1; j < n; j++)
                    {
                        if (p1[j, i] != 0)
                        {
                            for (int r = i; r < 2 * n; r++)
                            {
                                double temp = p1[j, r];
                                p1[j, r] = p1[i, r];
                                p1[i, r] = temp;
                            }
                        }
                        break;
                    }
                }
                //if (p1[i][i]==0) return 0;
            }
            //行变换为上三角矩阵
            double k;
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    k = (-1) * p1[j, i] / p1[i, i];
                    for (int r = i; r < 2 * n; r++)
                        p1[j, r] += k * p1[i, r];
                }
            }
            //行变换为下三角矩阵
            //double k=0;
            for (int i = n - 1; i >= 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    k = (-1) * p1[j, i] / p1[i, i];
                    for (int r = 0; r < 2 * n; r++)
                        p1[j, r] += k * p1[i, r];
                }
            }
            //化为单位阵
            for (int i = n - 1; i >= 0; i--)
            {
                k = p1[i, i];
                for (int j = 0; j < 2 * n; j++)
                    p1[i, j] /= k;
            }

            //拆分出逆矩阵
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    p2[i, j] = p1[i, n + j];
            }
        }
    }
}