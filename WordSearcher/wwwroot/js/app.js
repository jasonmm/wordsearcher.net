var app = angular.module('wordsearcher', ['ngRoute', 'ngResource']);

app.config(['$routeProvider', '$locationProvider', function($routeProvider, $locationProvider) {
    $locationProvider.html5Mode(true);
    $routeProvider
        .when('/', {
            templateUrl: '/partials/create-form.html',
            controller: 'CreateWordSearchCtrl'
        })
        .when('/show-grid', {
            templateUrl: '/partials/show-grid.html',
            controller: 'ShowGridCtrl'
        })
        ;
}]);

app.factory('GridSvc', function() {
    var service = {
        grid: [],
        wordList: [],
        title: ''
    };

    service.set = function(grid, wl, t) {
        service.grid = grid;
        service.wordList = wl;
        service.title = t;
    };

    service.getGrid = function() {
        return service.grid;
    };

    service.getWordList = function() {
        return service.wordList;
    };

    service.getTitle = function() {
        return service.title;
    };

    return service;
});

