app.service('deviceCommunicationService', ['$', '$rootScope', function ($, $rootScope) {
    var proxy = null;

    var initialize = function () {
        //Getting the connection object
        connection = $.hubConnection();

        //Creating proxy
        this.proxy = connection.createHubProxy('deviceCommunicationHub');

        this.proxy.on('registeredDevices', function (message) {
            $rootScope.$emit("registeredDevices", message);
        });

        this.proxy.on('connectedToDevice', function (message) {
            $rootScope.$emit("connectedToDevice", message);
        });

        this.proxy.on('connectionFailure', function (message) {
            $rootScope.$emit("connectionFailure", message);
        });

        //Starting connection
        connection.start();
    };

    var connectToDevice = function (agentId, deviceName) {
        this.proxy.invoke('connectToDevice', agentId, deviceName);
    };

    var forward = function (agentId, deviceName) {
        this.proxy.invoke('sendCommand', agentId, deviceName, 'forward');
    }

    var reverse = function (agentId, deviceName) {
        this.proxy.invoke('sendCommand', agentId, deviceName, 'reverse');
    }

    var left = function (agentId, deviceName) {
        this.proxy.invoke('sendCommand', agentId, deviceName, 'left');
    }

    var right = function (agentId, deviceName) {
        this.proxy.invoke('sendCommand', agentId, deviceName, 'right');
    }

    var stop = function (agentId, deviceName) {
        this.proxy.invoke('sendCommand', agentId, deviceName, 'stop');
    }

    return {
        initialize: initialize,
        connectToDevice: connectToDevice,
        forward: forward,
        reverse: reverse,
        left: left,
        right: right,
        stop: stop
    };
}]);