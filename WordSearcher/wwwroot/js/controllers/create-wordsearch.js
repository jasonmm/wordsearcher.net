app.controller('CreateWordSearchCtrl', ['$scope', '$resource', '$location', 'GridSvc',
    function($scope, $resource, $location, GridSvc) {
        $scope.sortType = 'alpha';
        $scope.showDate = true;
        $scope.uppercaseWords = true;
        $scope.rows = 20;
        $scope.columns = 20;
        $scope.wordList = '';
        $scope.creating = false;

        $scope.create = function() {
            var wordList = $scope.wordList.split("\n");

            $resource('/api/WordSearcher').save(
                {
                    Title: $scope.title,
                    Rows: $scope.rows,
                    Columns: $scope.columns,
                    SortType: $scope.sortType,
                    ShowDate: $scope.showDate,
                    UppercaseWords: $scope.uppercaseWords,
                    WordList: wordList
                },
                function(data) {
                    GridSvc.set(data.Grid, wordList, $scope.title);
                    $location.url('/show-grid');
                },
                function(response) {
                    console.log(response);
                }
            );
        };
    }]);