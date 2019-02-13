globalModule.controller("HomeController",
    ['$scope', 'homeRepository',
        function ($scope, homeRepository) {
            $scope.testScope = 5;

            $scope.getAllUsers = function () {
                var getAllUsersPromise = homeRepository.getAll().$promise;
                getAllUsersPromise.then(function (result) {
                    console.log(result);
                });
            }
        }

    ]);