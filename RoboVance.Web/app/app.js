var app = angular.module('RoboVance', ['ngRoute']) // TODO switch to UI router
app.value('$', $);
app.value('signalRServer', 'http://localhost:58028/');

app.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        .when('/', {
            controller: 'deviceConnectionCtrl',
            templateUrl: '/app/views/deviceConnection.html'
        })

    .otherwise({ redirectTo: '/' });

}]);