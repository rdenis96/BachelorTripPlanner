globalModule.controller("HeaderController",
    ['$scope', '$localStorage', 'homeRepository',
        function ($scope, $localStorage, homeRepository) {
            $scope.isLogged = $localStorage.TPUserId !== null && $localStorage.TPUserId !== undefined;
            $scope.init = function () {
                if ($scope.isLogged == true)
                    $scope.userId = $localStorage.TPUserId;
            };
        }

    ]);