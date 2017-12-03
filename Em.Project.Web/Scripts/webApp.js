var webApp = {

	//数据加载
	wait: function(){
	    var count = 0;
	    var html = [
	        '<div class = "local-loading-window">',
	            
	        '</div>',

	        '<div class = "local-loading-window-content">',
	            '<div class="loding-progress"></div>',
	        '</div>',
	    ];
	    this.show = function(){
            count++;
            if(1)
            {
                if ($(".local-loading-window").length == 0) {

                    $("body").append(html.join(''));
                }

                else
                    $(".local-loading-window,.local-loading-window-content").show();
            }
        };
	   	this.hide = function(){
    		count--;
			if (count <= 0) 
				$(".local-loading-window,.local-loading-window-content").hide();
    	}
    	return this;
	},

	net: function(){
		var noop = _.noop;
		var helper = function(type, url, data, callback, failer) {
			if (_.isFunction(data)) {
				callback = data, failer = callback, data = undefined;
			}

			if (!_.isFunction(callback)) callback = noop;
			if (!_.isFunction(failer)) failer = noop;

			this.wait().show();
			$.ajax({
				url: "/api/services/Service/" + url,
				method: type,
			    dataType:"json",
			    contentType:"application/json",
				data: data
			}).done(function(data) {
				this.wait().hide();
				if (data.success) {
					var datas = data.result;

					(callback || noop)(datas);
				} else{
					if (data.unAuthorizedRequest) {
						window.location = "/login/login.html";
					}
					log(data);
				};
			}.bind(webApp)).error(function(xhr, status) {

				this.wait().hide();

				(failer || noop)(xhr, status);

			}.bind(webApp));
		}.bind(webApp);
		var request = {
			post: function(url, data, callback, failer) {
				helper('POST', url, data, callback, failer);
			},

			get: function(url, data, callback, failer) {
				helper('GET', url, data, callback, failer);
			},

			delete: function(url, data, callback, failer) {
				if(confirm("确认删除数据？"))
					helper('DELETE', url, data, callback, failer);
			},

			put: function(url, data, callback, failer) {
				helper('PUT', url, data, callback, failer);
			}
		};

		return request;
	}
}
