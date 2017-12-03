//用错误提示，填充当前页面（清除当前页面内容，重新加载错误提示至当前页面）
//strTitle：错误标题
//strMessage：错误消息
var SendErrorInfo = function (strTitle, strMessage) {
    if ($.trim(strTitle) == "") {
        strTitle = "error页面有误";
    }
    if ($.trim(strMessage) == "") {
        strMessage = "当前页面有误，请联系管理员！<br/>给您带来的不便，敬请谅解！";
    }
    var strStyle = '<style>html,body,div,span,iframe,p,a,img,q,s,samp,small,strike,strong,sub,sup,tt,var,b,u,i,dl,dt,dd,ol,ul,li,form,label,table,tbody,thead, tr, th, td,embed {margin: 0;padding: 0;border: 0;font-size: 100%;font: inherit;outline: none;}';
    strStyle += ' html{ height: 100%; } ';
    strStyle += ' table{border-collapse: collapse;border-spacing: 0;}';
    strStyle += ' img {border: 0;max-width: 100%;}';
    strStyle += ' a{text-decoration: none}';
    strStyle += ' a:hover { text-decoration: underline }';
    strStyle += ' body {height: 100%;font-size: 62.5%;line-height: 1;font-family: Arial, Tahoma, Verdana, sans-serif;font-family: Helvetica, Arial, sans-serif;-webkit-font-smoothing: antialiased;overflow: hidden;}';
    strStyle += ' @-webkit-keyframes main {0% {-webkit-transform: scale3d(0.1, 0.1, 1);opacity: 0;}45% {-webkit-transform: scale3d(1.07, 1.07, 1);opacity: 1;}70% { -webkit-transform: scale3d(0.95, 0.95, 1) }	100% { -webkit-transform: scale3d(1, 1, 1) }}';
    strStyle += ' @-moz-keyframes main {0% {-moz-transform: scale(0.1, 0.1);opacity: 0;	}	45% {-moz-transform: scale(1.07, 1.07); opacity: 1;	}70% { -moz-transform: scale(0.95, 0.95) }100% { -moz-transform: scale(1, 1) }}';
    strStyle += ' .main {position: relative;width: 90%;margin: 0 auto;padding-top: 8%;animation: main .8s 1;animation-fill-mode: forwards;-webkit-animation: main .8s 1;-webkit-animation-fill-mode: forwards;-moz-animation: main .8s 1;-moz-animation-fill-mode: forwards;-o-animation: main .8s 1;-o-animation-fill-mode: forwards;-ms-animation: main .8s 1;-ms-animation-fill-mode: forwards;}';
    strStyle += ' .main .header h1 {position: relative;display: block;font: 50px \'TeXGyreScholaBold\', Arial, sans-serif;color: #0061a5;text-shadow: 2px 2px #f7f7f7;text-align: center;}';
    strStyle += ' .main .header h1 span.icon {position: relative;display: inline-block;top: -6px;margin: 0 10px 5px 0;background: #0061a5;width: 50px;height: 50px;-moz-box-shadow: 1px 2px white;-webkit-box-shadow: 1px 2px white;box-shadow: 1px 2px white;-webkit-border-radius: 50px;-moz-border-radius: 50px;border-radius: 50px;color: #dfdfdf;font-size: 46px;line-height: 48px;font-weight: bold;text-align: center;text-shadow: 0 0;}';
    strStyle += ' .main .content {position: relative;background: white;-moz-box-shadow: 0 0 0 3px #ededed inset, 0 0 0 1px #a2a2a2, 0 0 20px rgba(0,0,0,.15);-webkit-box-shadow: 0 0 0 3px #ededed inset, 0 0 0 1px #a2a2a2, 0 0 20px rgba(0,0,0,.15);box-shadow: 0 0 0 3px #ededed inset, 0 0 0 1px #a2a2a2, 0 0 20px rgba(0,0,0,.15);-webkit-border-radius: 5px;-moz-border-radius: 5px;border-radius: 5px;z-index: 5;width:100%;}';
    strStyle += ' .main .content p {position: relative;padding: 15px;font-size: 16px;line-height: 1.6em;color: #555;}';
    strStyle += '</style>';
    strStyle += ''

    var strHtml = '<div class="main zh" >';
    strHtml += '<table><tr><td width: 80%;> <header class="header"><h1><span class="icon">!</span>' + strTitle + '</h1></header>';
    strHtml += '<div class="content">';
    strHtml += '<p>' + strMessage + '</p>';
    strHtml += '</div></td>';
    strHtml += '<td style="width: 20%;">';
    strHtml += '<img src="' + bootPATH + 'Common/images/errorPage/Error.png" style="width: 100%;" />';
    strHtml += '</td></tr></table>';
    strHtml += '</div>'

    $("head").empty();//清除当前页面的head内容
    $("body").empty();//清除当前页面的BODY内容
    
    $(strStyle).appendTo("head");//添加显示内容至head中
    $(strHtml).appendTo("body");//添加显示内容至BODY中
}