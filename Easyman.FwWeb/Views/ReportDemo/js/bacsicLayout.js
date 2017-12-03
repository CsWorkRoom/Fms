$(function(){
	//报表脚本中的全屏按钮		
	$('#myTab li:first a').tab('show');
	$("#fullContent").click(function(){
		
		if($("#formScript div.topContent").hasClass("col-xs-9")){
			$("#formScript div.topContent").removeClass("col-xs-9").addClass("col-xs-12");
			$("#formScript div.rightContent").css("display","none");
			$("#formScript div.bottomContent").css("display","none");
		}else{
			$("#formScript div.topContent").removeClass("col-xs-12").addClass("col-xs-9");
			$("#formScript div.rightContent").css("display","");
			$("#formScript div.bottomContent").css("display","");
		}
		SetLayoutSize();
	});
	
	window.onresize = SetLayoutSize;

	//报表脚本中的全屏按钮结束
	
	$("#myTab-tab>li:first-child").click(function(){
		$("#myTab-tab>li:nth-child(4)").css("display","block").next().css("display","none");
	});
	$("#myTab-tab>li:nth-child(2)").click(function(){
		$("#myTab-tab>li:last-child").css("display","block").prev().css("display","none");
	});
	$("#myTab-tab>li:nth-child(3)").click(function(){
		$("#myTab-tab>li:last-child").css("display","none").prev().css("display","none");
	});
	
//	web报表
	$("[href='#web-inquiry']").click(function(){
		
		$("#webForm .rightContent").removeClass("none");
		
			$("#webForm div.inquiryBox").removeClass("none");
			
			$("#webForm .inquiryContent").css("display","block").parent().siblings().children("[class*='Content']").css("display","none")
		
		
	})
	$("[href='#web-function']").click(function(){
		$("#webForm .rightContent").removeClass("none");
		
			$("#webForm div.functionBox").removeClass("none");
			
		$("#webForm .functionContent").css("display","block").parent().siblings().children("[class*='Content']").css("display","none")
		
	})
	$("[href='#web-form']").click(function(){
		$("#webForm .rightContent").removeClass("none");
		
			$("#webForm div.formBox").removeClass("none");
			
		$("#webForm .formContent").css("display","block").parent().siblings().children("[class*='Content']").css("display","none")
		
	})
	$("[href='#web-chart']").click(function(){
		$("#webForm .rightContent").removeClass("none");
		
			$("#webForm div.chartBox").removeClass("none");
			
		$("#webForm .chartContent").css("display","block").parent().siblings().children("[class*='Content']").css("display","none")
		
	})
	
//	app报表
	$("[href='#app-inquiry']").click(function(){
		$("#appForm .rightContent").removeClass("none");
		
			$("#appForm div.inquiryBox").removeClass("none");
			
			$("#appForm .inquiryContent").css("display","block").parent().siblings().children("[class*='Content']").css("display","none")
		
		
	})
	$("[href='#app-function']").click(function(){
		$("#appForm .rightContent").removeClass("none");
		
			$("#appForm div.functionBox").removeClass("none");
			
		$("#appForm .functionContent").css("display","block").parent().siblings().children("[class*='Content']").css("display","none")
		
	})
	$("[href='#app-form']").click(function(){
		$("#appForm .rightContent").removeClass("none");
		
			$("#appForm div.formBox").removeClass("none");
			
		$("#appForm .formContent").css("display","block").parent().siblings().children("[class*='Content']").css("display","none")
		
	})
	$("[href='#app-chart']").click(function(){
		$("#appForm .rightContent").removeClass("none");
		
			$("#appForm div.chartBox").removeClass("none");
			
		$("#appForm .chartContent").css("display","block").parent().siblings().children("[class*='Content']").css("display","none")
		
	})
	$(".title i").click(function(){
		if($(this).parent().siblings().css("display")=="none"){
			$(this).parent().parent("[class*='Box']").addClass("none");
		}else{
			$(this).parent().parent("[class*='Box']").addClass("none");
			$(this).parent().parent().parent().siblings(".rightContent").addClass("none")
		}
				
	});
	
	function SetLayoutSize(){
		var areaHeight=$(".areaContent").height();
		var myTabHeight=$("#myTab").height();
		var screebHeight=$("#formScript div.topContent").hasClass("col-xs-9") ? 200 : areaHeight-myTabHeight-136;
		$("#formScript div.topContent .textContent").height(screebHeight);
		//		rdlc高度
		var minRdlcTextHeight=areaHeight-294;
		$("#rdlc .bottomContent").height(minRdlcTextHeight);
	}
})
