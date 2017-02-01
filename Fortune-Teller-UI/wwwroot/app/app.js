angular.module('buzzgraph', ['ngRoute']).config(function ($routeProvider) {

    $routeProvider.when('/', {
        templateUrl: 'fortune.html',
        controller: 'buzzgraph'
    })

}).controller('buzzgraph', function ($scope, $http) {

    $http.get('buzzgraph').success(function (data) {
        console.log(data);
        $scope.buzzgraph = data.buzzGraphResultList;
    });

});