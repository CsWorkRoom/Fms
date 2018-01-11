var setting = {
    view: {
        selectedMulti: false
    },
    data: {
        key: {
            title: "title"
        },
        simpleData: {
            enable: true
        }
    },
    callback: {
        //beforeClick: beforeClick,
        //beforeCollapse: beforeCollapse,
        //beforeExpand: beforeExpand,
        //onCollapse: onCollapse,
        //onExpand: onExpand
        onClick: zTreeOnClick
    }
};

//点击节点后触发
function zTreeOnClick(event, treeId, treeNode) {
    //debugger;
    //根据当前节点获取对应的内容
    switch (treeNode.nodeType) {
        case "computer"://终端
            //debugger;
            getChildByComputer(treeNode.id.substring(treeNode.id.indexOf("_") + 1));
            break;
        case "folder"://共享目录
            getChildByFolder(treeNode.id.substring(treeNode.id.indexOf("_") + 1));
            break;
        case "file"://文件
            if (treeNode.isFolder)//为文件夹时
            {
                getChildByFile(treeNode.id.substring(treeNode.id.indexOf("_") + 1));
            }
            else {
                alert("Current file is not a folder");
            }
            break;
    }

};
