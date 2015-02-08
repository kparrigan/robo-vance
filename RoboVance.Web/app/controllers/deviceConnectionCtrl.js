app.controller('deviceConnectionCtrl', ['$scope', 'deviceCommunicationService', '$rootScope', function ($scope, deviceCommunicationService, $rootScope) {
    $scope.showControls = false;
    $scope.showConnection = true;
    $scope.commandText = 'No Command';

    deviceCommunicationService.initialize();

    $scope.connectToDevice = function (agentId, deviceName) {
        $scope.currentAgentId = agentId;
        $scope.currentDevice = deviceName;
        deviceCommunicationService.connectToDevice(agentId, deviceName);

    };

    updateDevices = function (deviceData) {
        $scope.agentDevices = deviceData;
    }

    $scope.forward = function () {
        $scope.commandText = 'Forward...';
        deviceCommunicationService.forward($scope.currentAgentId, $scope.currentDevice);
    }

    $scope.reverse = function () {
        $scope.commandText = 'Reverse...';
        deviceCommunicationService.reverse($scope.currentAgentId, $scope.currentDevice);
    }

    $scope.left = function () {
        $scope.commandText = 'Left...';
        deviceCommunicationService.left($scope.currentAgentId, $scope.currentDevice);
    }

    $scope.right = function () {
        $scope.commandText = 'Right...';
        deviceCommunicationService.right($scope.currentAgentId, $scope.currentDevice);
    }

    $scope.stop = function () {
        $scope.commandText = 'Stop';
        deviceCommunicationService.stop($scope.currentAgentId, $scope.currentDevice);
    }

    $scope.$parent.$on("registeredDevices", function (e, message) {
        $scope.$apply(function () {
            updateDevices(message)
        });
    });

    $scope.$parent.$on("connectedToDevice", function (e, message) {
        $scope.$apply(function () {
            if (message.AgentId === $scope.currentAgentId
                && message.DeviceName === $scope.currentDevice)
            {
                $scope.showControls = true;
                $scope.showConnection = false;
            }
        });
    });

    $scope.$parent.$on("connectionFailure", function (e, message) {

        $scope.$apply(function () {
            var foo = message;
        });
    });

}]);

