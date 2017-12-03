(function($){
$.fn.Huploadify = function(opts){
    var itemTemp = '<div id="${fileID}" class="uploadify-queue-item"><div class="uploadify-progress"><div class="uploadify-progress-bar"></div></div><span class="up_filename" id="filename${fileID}">${fileName}</span><span class="uploadbtn">上传</span><span class="delfilebtn">删除</span></div>';
	var defaults = {
		fileTypeExts:'',//允许上传的文件类型，格式'*.jpg;*.doc'
		uploader:'',//文件提交的地址
		auto:false,//是否开启自动上传
		method:'post',//发送请求的方式，get或post
		multi:true,//是否允许选择多个文件
		formData:null,//发送给服务端的参数，格式：{key1:value1,key2:value2}
		fileObjName:'file',//在后端接受文件的参数名称，如PHP中的$_FILES['file']
		fileSizeLimit:2048,//允许上传的文件大小，单位KB
		showUploadedPercent:true,//是否实时显示上传的百分比，如20%
		showUploadedSize:false,//是否实时显示已上传的文件大小，如1M/2M
		buttonText:'选择文件',//上传按钮上的文字
		removeTimeout: 1000,//上传完成后进度条的消失时间
		itemTemplate:itemTemp,//上传队列显示的模板
		onUploadStart:null,//上传开始时的动作
		onUploadSuccess:null,//上传成功的动作
		onUploadComplete:null,//上传完成的动作
		onUploadError:null, //上传失败的动作
		onInit:null,//初始化时的动作
		onCancel:null//删除掉某个文件后的回调函数，可传入参数file
	}
		
	var option = $.extend(defaults,opts);
	
	//将文件的单位由bytes转换为KB或MB，若第二个参数指定为true，则永远转换为KB
	var formatFileSize = function(size,byKB){
		if (size> 1024 * 1024&&!byKB){
			size = (Math.round(size * 100 / (1024 * 1024)) / 100).toString() + 'MB';
		}
		else{
			size = (Math.round(size * 100 / 1024) / 100).toString() + 'KB';
		}
		return size;
	}
	//根据文件序号获取文件
	var getFile = function(index,files){
		for(var i=0;i<files.length;i++){	   
		  if(files[i].index == index){
			  return files[i];
			}
		}
		return false;
	}
	
	//将输入的文件类型字符串转化为数组,原格式为*.jpg;*.png
	var getFileTypes = function(str){
		var result = [];
		var arr1 = str.split(";");
		for(var i=0,len=arr1.length;i<len;i++){
			result.push(arr1[i].split(".").pop());
		}
		return result;
	}


    //将输入的文件类型字符串转化为数组的MIME,原格式为*.jpg;*.png
	var GetFileMime = function (str) {
	    var result = [];
	    var arr1 = str.split(";");
	    for (var i = 0, len = arr1.length; i < len; i++) {
	        var strMime = arr1[i].split(".").pop();
	        if (strMime != null && $.trim(strMime) != "") {
	            strMime = $.trim(strMime).toLowerCase();
	            switch (strMime) {
	                case "3gpp":
	                    strMime = "audio/3gpp, video/3gpp";
	                    break;
	                case "ac3":
	                    strMime = "audio/ac3";
	                    break;
	                case "asf":
	                    strMime = "allpication/vnd.ms-asf";
	                    break;
	                case "au":
	                    strMime = "audio/basic";
	                    break;
	                case "css":
	                    strMime = "text/css";
	                    break;
	                case "csv":
	                    strMime = "text/csv";
	                    break;
	                case "doc": case "dot":
	                    strMime = "application/msword";
	                    break;
	                case "dtd":
	                    strMime = "application/xml-dtd";
	                    break;
	                case "dwg":
	                    strMime = "image/vnd.dwg";
	                    break;
	                case "dxf":
	                    strMime = "image/vnd.dxf";
	                    break;
                    case "xpm":
                        strMime = "image/x-xpixmap";
                        break;
	                case "dib": case "bmp":
	                    strMime = "image/bmp";
                        break;
                    case "ico":
                        strMime = "image/x-icon";
                        break;
                    case "gif":
                        strMime = "image/gif";
                        break;
	                case "jpeg": case "jpe": case "jpg":
	                    strMime = "image/jpeg";
	                    break;
	                case "png": case "pnz":
	                    strMime = "image/png";
	                    break;
	                case "txt": case "h": case "map": case "cnf": case "asm":
	                    strMime = "text/plain";
	                    break;
	                case "dll":
	                    strMime = "application/x-msdownload";
	                    break;
	                case "htm": case "html": case "cshtml":
	                    strMime = "text/html";
	                    break;
	                case "pot": case "ppt":
	                    strMime = "application/vnd.ms-powerpoint";
	                    break;
	                case "rtf":
	                    strMime = "application/rtf, text/rtf";
	                    break;
	                case "svf":
	                    strMime = "image/vnd.svf";
	                    break;
	                case "tiff":
	                    strMime = "image/tiff";
	                    break;
	                case "xla": case "xls": case "xlt": case "xlc": case "xlm": case "xlw":
	                    strMime = "application/vnd.ms-excel";
	                    break;
	                case "xml": case "xslt": case "xsf": case "mno": case "xsl": case "dtd": case "wsdl": case "disco": case "xsd":
	                    strMime = "text/xml, application/xml";
	                    break;
	                case "xlsx":
	                    strMime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
	                    break;
	                case "docx":
	                    strMime = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
	                    break;
	                case "docm":case "dotm":
                        strMime = "application/vnd.ms-word.template.macroEnabled.12";
                        break;
	                case "xlsm":
	                    strMime = "application/vnd.ms-excel.sheet.macroEnabled.12";
                        break;
                    case "wav":
                        strMime = "audio/wav";
                        break;
	                case "mpv2": case "mpeg": case "m1v": case "mpa": case "mpe": case "mp2":
                        strMime = "video/mpeg";
                        break;                        
	                case "pptx":
	                    strMime = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
	                    break;
	                case "zip":
	                    strMime = "application/x-zip-compressed";
	                    break;
	                case "swf":
	                    strMime = "application/x-shockwave-flash";
	                    break;
                    case "flv":
                        strMime = "flv-application/octet-stream";
                        break;
	                case "avi":
	                    strMime = "video/x-msvideo";
                        break;	                    
	                case "mpv2": case "mpeg": case "m1v": case "mpa": case "mpe": case "mp2":
	                    strMime = "video/mpeg";
	                    break;
	                case "psm": case "java": case "eot":
	                    strMime = "application/octet-stream";
	                    break;
	                case "ttf": case "csv": case "aaf": case "lpk": case "bin": case "thn": case "mso": case "pfm": case "toc": case "mdp": case "ics": case "chm": case "asi": case "afm": case "prm": case "mix": case "lzh": case "hhk": case "pfb": case "fla": case "qxd": case "dwp": case "smi": case "sea": case "hhp": case "aca": case "cur": case "rar": case "psd": case "inf": case "emz": case "dsp": case "snp": case "cab": case "prx": case "pcz":
	                    strMime = "application/octet-stream";
	                    break;
	            }
	        }
	        result.push(strMime);
	    }
	    return result;
	}
	
	this.each(function(){
		var _this = $(this);
		//先添加上file按钮和上传列表
		var instanceNumber = $('.uploadify').length+1;
		var inputStr = '<input id="select_btn_'+instanceNumber+'" class="selectbtn" style="display:none;" type="file" name="fileselect[]"';
		inputStr += option.multi ? ' multiple' : '';
		inputStr += ' accept="';
		inputStr += GetFileMime(option.fileTypeExts).join(",");
		inputStr += '"/>';
		inputStr += '<a id="file_upload_' + instanceNumber + '-button" href="javascript:void(0)" class="uploadify-button" style="text-decoration:none;">';
		inputStr += option.buttonText;
		inputStr += '</a>';
		var uploadFileListStr = '<div id="file_upload_'+instanceNumber+'-queue" class="uploadify-queue"></div>';
		_this.append(inputStr+uploadFileListStr);	
		
		
		//创建文件对象
	  var fileObj = {
		  fileInput: _this.find('.selectbtn'),				//html file控件
		  uploadFileList : _this.find('.uploadify-queue'),
		  url: option.uploader,						//ajax地址
		  fileFilter: [],					//过滤后的文件数组
		  filter: function(files) {		//选择文件组的过滤方法
			  var arr = [];
			  var typeArray = getFileTypes(option.fileTypeExts);
			  if(typeArray.length>0){
				  for(var i=0,len=files.length;i<len;i++){
				  	var thisFile = files[i];
				  		if(parseInt(formatFileSize(thisFile.size,true))>option.fileSizeLimit){
				  			alert('文件'+thisFile.name+'大小超出限制！');
				  			continue;
				  		}
						if($.inArray(thisFile.name.split('.').pop(),typeArray)>=0){
							arr.push(thisFile);	
						}
						else{
							alert('文件'+thisFile.name+'类型不允许！');
						}  	
					}	
				}
			  return arr;  	
		  },
		  //文件选择后
		  onSelect: function(files){
				for(var i=0,len=files.length;i<len;i++){
					var file = files[i];
					//处理模板中使用的变量
					var $html = $(option.itemTemplate.replace(/\${fileID}/g,'fileupload_'+instanceNumber+'_'+file.index).replace(/\${fileName}/g,file.name).replace(/\${fileSize}/g,formatFileSize(file.size)).replace(/\${instanceID}/g,_this.attr('id')));
					//如果是自动上传，去掉上传按钮
					if(option.auto){
						$html.find('.uploadbtn').remove();
					}
					this.uploadFileList.append($html);
					
					//判断是否显示已上传文件大小
					if(option.showUploadedSize){
						var num = '<span class="progressnum"><span class="uploadedsize">0KB</span>/<span class="totalsize">${fileSize}</span></span>'.replace(/\${fileSize}/g,formatFileSize(file.size));
						$html.find('.uploadify-progress').after(num);
					}
					
					//判断是否显示上传百分比	
					if(option.showUploadedPercent){
						var percentText = '<span class="up_percent">0%</span>';
						$html.find('.uploadify-progress').after(percentText);
					}
					//判断是否是自动上传
					if(option.auto){
						this.funUploadFile(file);
					}
					else{
						//如果配置非自动上传，绑定上传事件
					 	$html.find('.uploadbtn').on('click',(function(file){
					 			return function(){fileObj.funUploadFile(file);}
					 		})(file));
					}
					//为删除文件按钮绑定删除文件事件
			 		$html.find('.delfilebtn').on('click',(function(file){
					 			return function(){fileObj.funDeleteFile(file.index);}
					 		})(file));
			 	}

			 
			},				
		  onProgress: function(file, loaded, total) {
				var eleProgress = _this.find('#fileupload_'+instanceNumber+'_'+file.index+' .uploadify-progress');
				var percent = (loaded / total * 100).toFixed(2) +'%';
				if(option.showUploadedSize){
					eleProgress.nextAll('.progressnum .uploadedsize').text(formatFileSize(loaded));
					eleProgress.nextAll('.progressnum .totalsize').text(formatFileSize(total));
				}
				if(option.showUploadedPercent){
					eleProgress.nextAll('.up_percent').text(percent);	
				}
				eleProgress.children('.uploadify-progress-bar').css('width',percent);
	  	},		//文件上传进度

		  /* 开发参数和内置方法分界线 */
		  
		  //获取选择文件，file控件
		  funGetFiles: function(e) {	  
			  // 获取文件列表对象
			  var files = e.target.files;
			  //继续添加文件
			  files = this.filter(files);
			  for(var i=0,len=files.length;i<len;i++){
			  	this.fileFilter.push(files[i]);	
			  }
			  this.funDealFiles(files);
			  return this;
		  },
		  
		  //选中文件的处理与回调
		  funDealFiles: function(files) {
			  var fileCount = _this.find('.uploadify-queue .uploadify-queue-item').length;//队列中已经有的文件个数
			  for(var i=0,len=files.length;i<len;i++){
				  files[i].index = ++fileCount;
				  files[i].id = files[i].index;
				  }
			  //执行选择回调
			  this.onSelect(files);
			  
			  return this;
		  },
		  
		  //删除对应的文件
		  funDeleteFile: function (index) {
		      var removeItem = index-1;
		      var fileListId = $("#FileId").val();
		      var fileIds = fileListId.split(',');
              //删除对应ID
		      //fileIds = $.grep(fileIds, function (value) {
		      //    return value != fileIds[removeItem];
		      //});
		      //修改对应ID
		      fileIds[removeItem] = "删除";
		      var fileListIds = "";
		      if (fileIds.length == 0) { $("#FileId").val(); }
		      else if (fileIds.length == 1) { $("#FileId").val(fileIds); }
		      else {
		          for (var i = 0; i < fileIds.length; i++) {
		              if (i == 0) {
		                  fileListIds = fileIds[i];
		              } else {
		                  fileListIds = fileListIds + "," + fileIds[i];
		              }
		          }
		          $("#FileId").val(fileListIds);
		      }
		      
		      //    for (var i = 1; i <= fileIds.length ; i++) {
		      //    if (index == i) {

		      //    } else {
		      //        if (fileListIds = "") {
		      //            fileListIds = fileIds[i];
		      //        } else {
		      //            fileListIds = fileListIds + "," + fileIds[i];
		      //        }
		      //    }
		      //}
		     

			  for (var i = 0,len=this.fileFilter.length; i<len; i++) {
					  var file = this.fileFilter[i];
					  if (file.index == index) {
						  this.fileFilter.splice(i,1);
						  _this.find('#fileupload_'+instanceNumber+'_'+index).fadeOut();
						  option.onCancel&&option.onCancel(file);	
						  break;
					  }
			  }
			 
			  return this;
		  },
		  
		  //文件上传
		  funUploadFile: function(file) {
			  var xhr = false;
			  try{
				 xhr=new XMLHttpRequest();//尝试创建 XMLHttpRequest 对象，除 IE 外的浏览器都支持这个方法。
			  }catch(e){	  
				xhr=ActiveXobject("Msxml12.XMLHTTP");//使用较新版本的 IE 创建 IE 兼容的对象（Msxml2.XMLHTTP）。
			  }
			  
			  if (xhr.upload) {
				  // 上传中
				  xhr.upload.addEventListener("progress", function(e) {
					  fileObj.onProgress(file, e.loaded, e.total);
				  }, false);
	  
				  // 文件上传成功或是失败
				  xhr.onreadystatechange = function(e) {
					  if (xhr.readyState == 4) {
						  if (xhr.status == 200) {
							  //校正进度条和上传比例的误差
							  var thisfile = _this.find('#fileupload_'+instanceNumber+'_'+file.index);
							  thisfile.find('.uploadify-progress-bar').css('width','100%');
								option.showUploadedSize&&thisfile.find('.uploadedsize').text(thisfile.find('.totalsize').text());
								option.showUploadedPercent&&thisfile.find('.up_percent').text('100%');

							  option.onUploadSuccess&&option.onUploadSuccess(file, xhr.responseText);
							  //在指定的间隔时间后删掉进度条
							  setTimeout(function(){
							  	_this.find('#fileupload_'+instanceNumber+'_'+file.index).fadeOut();
							  },option.removeTimeout);
						  } else {
							  option.onUploadError&&option.onUploadError(file, xhr.responseText);		
						  }
						  option.onUploadComplete&&option.onUploadComplete(file,xhr.responseText);
						  //清除文件选择框中的已有值
						  fileObj.fileInput.val('');
					  }
				  };
	  
	  			option.onUploadStart&&option.onUploadStart();	
				  // 开始上传
				  xhr.open(option.method, this.url, true);
				  xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");
				  var fd = new FormData();
				  fd.append(option.fileObjName,file);
				  if(option.formData){
				  	for(key in option.formData){
				  		fd.append(key,option.formData[key]);
				  	}
				  }
				  
				  xhr.send(fd);
			  }	
			  
				  
		  },
		  
		  init: function() {	  
			  //文件选择控件选择
			  if (this.fileInput.length>0) {
				  this.fileInput.change(function(e) { 
				  	fileObj.funGetFiles(e); 
				  });	
			  }
			  
			  //点击上传按钮时触发file的click事件
			  _this.find('.uploadify-button').on('click',function(){
				  _this.find('.selectbtn').trigger('click');
				});
			  
			  option.onInit&&option.onInit();
		  }
  	};

		//初始化文件对象
		fileObj.init();
	}); 
}	

})(jQuery)