globalModule.controller("HomeController",
    ['$scope', '$window', '$localStorage', 'homeRepository', 'toastr',
        function ($scope, $window, $localStorage, homeRepository, toastr) {
            $scope.userId = undefined;
            $scope.suggestedInterests = [];
            $scope.suggestedInterestsLoaded = false;
            $scope.randomInterests = [];
            $scope.randomInterestsLoaded = false;

            $scope.init = function () {
                $scope.userId = $localStorage.TPUserId;
                $scope.initSuggestedInterests();
            };

            $scope.initSuggestedInterests = function () {
                var getSuggestedInterestsPromise = homeRepository.getSuggestedInterests({ userId: $scope.userId }).$promise;
                getSuggestedInterestsPromise.then(function (result) {
                    $scope.suggestedInterests = result;
                    $scope.suggestedInterestsLoaded = true;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });

                var getRandomInterestsPromise = homeRepository.getRandomInterests().$promise;
                getRandomInterestsPromise.then(function (result) {
                    $scope.randomInterests = result;
                    $scope.randomInterestsLoaded = true;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.init();
        }

    ]);