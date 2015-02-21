app.service('deviceCommunicationService', ['$', '$rootScope', function ($, $rootScope) {
    var proxy = null;

    var initialize = function () {
        //Getting the connection object
        connection = $.hubConnection();

        //Creating proxy
        proxy = connection.createHubProxy('deviceCommunicationHub');

        proxy.on('registeredDevices', function (message) {
            $rootScope.$emit("registeredDevices", message);
        });

        proxy.on('connectedToDevice', function (message) {
            $rootScope.$emit("connectedToDevice", message);
        });

        proxy.on('connectionFailure', function (message) {
            $rootScope.$emit("connectionFailure", message);
        });

        //Starting connection
        connection.start();
    };

    var connectToDevice = function (agentId, deviceName) {
        proxy.invoke('connectToDevice', agentId, deviceName);
    };

    var forward = function (agentId, deviceName) {
        proxy.invoke('sendCommand', agentId, deviceName, 'forward');
    }

    var reverse = function (agentId, deviceName) {
        proxy.invoke('sendCommand', agentId, deviceName, 'reverse');
    }

    var left = function (agentId, deviceName) {
        proxy.invoke('sendCommand', agentId, deviceName, 'left');
    }

    var right = function (agentId, deviceName) {
        proxy.invoke('sendCommand', agentId, deviceName, 'right');
    }

    var stop = function (agentId, deviceName) {
        proxy.invoke('sendCommand', agentId, deviceName, 'stop');
    }

    var powerDown = function (agentId, deviceName) {
        proxy.invoke('sendCommand', agentId, deviceName, 'powerDown');
    }

    return {
        initialize: initialize,
        connectToDevice: connectToDevice,
        forward: forward,
        reverse: reverse,
        left: left,
        right: right,
        stop: stop,
        powerDown: powerDown
    };
}]);