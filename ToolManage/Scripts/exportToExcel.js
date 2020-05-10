//获取下载日期
function getDate(now) {
	let year = now.getFullYear()
	let month = now.getMonth() + 1
	let day = now.getDate()
	if(month < 10) {
		month = '0' + month
	}
	if(day <10) {
		day = '0' + day
	}
	return year + month + day
}

//下载
function download(str,downloadType) {
	const now = new Date()
	let uri = 'data:text/csv;charset=utf-8,\ufeff' + encodeURIComponent(str)
	let link = document.createElement("a")
	link.href = uri
	link.download = downloadType + getDate(now) + '.csv'
	document.body.appendChild(link)
	link.click();
	document.body.removeChild(link)
}

//导出维修列表
function handleRepairExport() {
	try {
		$.ajax({
			// http://yapi.hdutu.xyz/mock/44/tsm/repairList
			url: 'http://yapi.hdutu.xyz/mock/44/tsm/repairList',
			dataType: 'json',
			type: 'GET',
			success: function(res) {
				let exportData = res
				let str = `Code,Location,CreateDate,Remark\n`
				for (let i = 0; i < exportData.length; i++) {
					for (let item in exportData[i]) {
						str += `${exportData[i][item] + '\t'},`
					}
					str += '\n'
				}
				download(str,'维修列表')
			},
			error: function() {
				alert('后台繁忙请重试')
			}
		})
	}catch(e) {
		alert('下载出现异常'+e)
	}
	
}

// 导出夹具定义
function handleDefineExport() {
	try {
		$.ajax({
			url: 'http://yapi.hdutu.xyz/mock/44/tsm/toolClassList',
			dataType: 'json',
			type: 'GET',
			success: function(res) {
				let exportData = res
				let str = `ID,Workcell,FamilyNo,Code,Name,ModelNo,PartNo,UseFor,UPL,OwnerId,OwnerName,PMPeriod,CreateTime\n`
				for (let i = 0; i < exportData.length; i++) {
					for (let item in exportData[i]) {
						str += `${exportData[i][item] + '\t'},`
					}
					str += '\n'
				}
				download(str,'夹具定义')
			},
			error: function(XMLHttpRequest, textStatus, errorThrown) {
				alert('系统繁忙请重试')
			}
		})
		
	} catch(e) {
		alert('下载出现异常'+e)
	}
}

//导出夹具实体
function handleToolExport() {
	try {
		$.ajax({
			url: 'http://yapi.hdutu.xyz/mock/44/tsm/toolUnitList',
			dataType: 'json',
			type: 'GET',
			success: function(res) {
				let exportData = res
				let str = `Code,SeqId,BillNo,RegDate,UserCount,Location\n`
				for (let i = 0; i < exportData.length; i++) {
					for (let item in exportData[i]) {
						str += `${exportData[i][item] + '\t'},`
					}
					str += '\n'
				}
				download(str,'夹具实体')
			},
			error: function(XMLHttpRequest, textStatus, errorThrown) {
				alert('系统繁忙请重试')
			}
		})
		
	} catch(e) {
		alert('下载出现异常'+e)
	}
}