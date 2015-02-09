app.controller('deviceCommandCtrl', ['$scope', 'deviceCommunicationService', '$rootScope', '$routeParams', function ($scope, deviceCommunicationService, $rootScope, $routeParams) {
    $scope.commandText = 'No Command';
    $scope.agentId = $routeParams.agentId;
    $scope.deviceName = $routeParams.deviceName;

    deviceCommunicationService.initialize();

    $scope.forward = function () {
        $scope.commandText = 'Forward...';
        deviceCommunicationService.forward($scope.agentId, $scope.deviceName);
    }

    $scope.reverse = function () {
        $scope.commandText = 'Reverse...';
        deviceCommunicationService.reverse($scope.agentId, $scope.deviceName);
    }

    $scope.left = function () {
        $scope.commandText = 'Left...';
        deviceCommunicationService.left($scope.agentId, $scope.deviceName);
    }

    $scope.right = function () {
        $scope.commandText = 'Right...';
        deviceCommunicationService.right($scope.agentId, $scope.deviceName);
    }

    $scope.powerDown = function () {
        $scope.commandText = 'Power Down';
        deviceCommunicationService.powerDown($scope.agentId, $scope.deviceName);
    }

    $scope.stop = function () {
        $scope.commandText = 'Stop';
        deviceCommunicationService.stop($scope.agentId, $scope.deviceName);
    }
}]);