/*自动存盘 今天事情*/
function day_save() {

    var dm_title = $("#dm_title").val();
    var dm_body = $("#dm_body").val();
    var dm_Time = $("#Time").val();
    $.post("/api/DaySave", {
        dm_title: dm_title, dm_body: dm_body, dm_Time: dm_Time

    }, function (data) {
        if (data != "成功") {
            alerts("数据没有保存成功");
        }
    });
}
/*加载 今天任务,任务列表等信息  @Url.Action("ShopStatePost", "huaGen")*/
function DayLoad() {

    $.post('/api/DayGet', { Time: $("#Time").val() }, function (data) {

        data = eval(data);

        $("#dm_title").val(data[0].dm_title);
        $("#dm_body").val(data[0].dm_body);
    }, "json");
}



/*辅助函数*/
function alerts(tt) {
    $.messager.alert('信息提示', tt);
}
/*比较两个时期的大小.如果D1大于D2就返回false*/
function dateCompare(date1, date2) {
    date1 = date1.replace(/\-/gi, "/");
    date2 = date2.replace(/\-/gi, "/");
    var time1 = new Date(date1).getTime();
    var time2 = new Date(date2).getTime();
    if (time1 < time2) {
        return true;
    }
    return false;
}


/*分界线*/
/**/
function compareTime(tit, txt, url) {
    $.messager.confirm(tit, txt, function (r) {
        if (r) {
            location.href = url;
        }
    });
}


/*封装弹出窗口*/
function GetInfto(ti, mess) {
    $.messager.alert(ti, mess);
}

/*标签说明
1.创建一个没有带刷新的
2.创建一个带刷新的
3.创建一个没有带关闭的
*/
//增加一个标签
function addTab(titles, url) {

    //判断当前标签是否存在/*这个在DOME中是没有的,要自己补上来*/
    if ($('#tt').tabs('exists', titles)) {
        $('#tt').tabs('select', titles)/*当前有标签就激活他的状态*/
        UpTab(titles, url); /*激活时刷新该页面*/
    }
    else {
        $('#tt').tabs('add', {
            title: titles,
            href: url, //外部加载的URL 
            closable: false,
            tools: [{
                iconCls: 'icon-mini-refresh',
                handler: function n() {
                    getSelected();
                }
            }]
        });
    }
}


//增加一个标签(带刷新窗口,一般是添加页面")
function addTab2(titles, url) {
    //判断当前标签是否存在/*这个在DOME中是没有的,要自己补上来*/
    if ($('#tt').tabs('exists', titles)) {
        $('#tt').tabs('select', titles)/*当前有标签就激活他的状态*/
        var tab = $('#tt').tabs('getSelected'); /*更新数据*/
        $('#tt').tabs('update', {
            tab: tab,
            options: {
                title: titles,
                href: url,
                cache: false
            }
        });

    }
    else {
        $('#tt').tabs('add', {
            title: titles,

            // content: '<iframe   scrolling="no" frameborder="0" width="100%" height="100%" src="' + url + '"> </iframe>', //内容
            //  content: $.get('home/ReadASCX', function (data) { return data; }),
            href: url, //外部加载的URL 
            closable: true,
            tools: [{
                iconCls: 'icon-mini-refresh',
                handler: function n() {
                    getSelected();
                }
            }]
        });
    }
}



//增加一个标签(没有关闭的按钮)
function addTab3(titles, url) {
    //判断当前标签是否存在
    if ($('#tt').tabs('exists', titles)) {
        $('#tt').tabs('select', titles)/*当前有标签就激活他的状态*/
    }
    else {
        $('#tt').tabs('add', {
            title: titles,
            href: url, //外部加载的URL 
            // content: '<iframe   scrolling="no" frameborder="0" width="100%" height="100%" src="' + url + '"> </iframe>', //内容
            //  content: $.get('home/ReadASCX', function (data) { return data; }), 
            //  iconCls: 'icon-reload',//设置图标
            closable: false,
            tools: [{
                iconCls: 'icon-mini-refresh',
                handler: function n() {
                    getSelected();
                }
            }]
        });
    }
}

//更新Tabs
function UpTab(titles, url) {
    var tab = $('#tt').tabs('getSelected');
    $('#tt').tabs('update', {
        tab: tab,
        options: {
            title: titles,
            href: url,
            cache: false
            //  content: $.get(url)
        }
    });
}

//刷新
function getSelected() {
    var tab = $('#tt').tabs('getSelected'); /*获取当前的tab信息*/
    $('#tt').tabs('update', {
        tab: tab,
        options: {
            title: tab.panel('options').title,
            href: tab.panel('options').href, // 使用href会导致页面加载两次，所以使用content代替
            cache: false /*这该死的属性不设置成false就不能用href.只能用那个content属性*/
        }
    });
}


//关闭窗口
function closeTab() {
    var tab = $('#tt').tabs('getSelected');  /*获取当前的tab信息*/
    $("#tt").tabs("close", tab.panel('options').tab.text());

}
 