app.controller('deviceConnectionCtrl', ['$scope', 'deviceCommunicationService', '$rootScope', '$location', '$filter', 'usSpinnerService', function ($scope, deviceCommunicationService, $rootScope, $location, $filter, usSpinnerService) {
    $scope.statusText = 'Choose a device to connect to.';

    deviceCommunicationService.initialize();

    $scope.connectToDevice = function (agentId, deviceName) {
        usSpinnerService.spin('connectionSpinner');
        $scope.statusText = 'Connecting to device: ' + deviceName;
        $scope.currentAgentId = agentId;
        $scope.currentDevice = deviceName;
        deviceCommunicationService.connectToDevice(agentId, deviceName);
    };

    updateDevices = function (deviceData) {
        $scope.flattenedDevices = $filter('flattenDevices')(deviceData);
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
                usSpinnerService.stop('connectionSpinner');
                $location.path('/command/' + $scope.currentAgentId + /device/ + $scope.currentDevice, false);
            }
        });
    });

    $scope.$parent.$on("connectionFailure", function (e, message) {

        $scope.$apply(function () {
            usSpinnerService.stop('connectionSpinner');
            $scope.statusText = 'Failed to connect to device: ' + deviceName;
        });
    });

}]);

