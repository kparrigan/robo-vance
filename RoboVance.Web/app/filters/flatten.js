app.filter('flattenDevices', function () {
    return function (agentDevices) {
        var i;
        var j;
        var flatList = [];

        if (agentDevices) {
            for (i = 0; i < agentDevices.length; i++) {
                var agent = agentDevices[i];
                for (j = 0; j < agent.devices.length; j++) {
                    var item = {};
                    item.agentId = agent.id;
                    item.deviceName = agent.devices[j]
                    flatList.push(item);
                }
            }
        }

        return flatList;
    };
});