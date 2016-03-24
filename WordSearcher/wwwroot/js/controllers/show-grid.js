app.controller('ShowGridCtrl', ['$scope', '$resource', '$location', 'GridSvc',
    function($scope, $resource, $location, GridSvc) {
        $scope.grid = GridSvc.getGrid();
        $scope.wordList = GridSvc.getWordList();
        $scope.title = GridSvc.getTitle();
    }]);