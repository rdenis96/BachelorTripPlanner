globalModule.controller("HomeController",
    ['$scope', '$window', '$localStorage', 'homeRepository', 'toastr',
        function ($scope, $window, $localStorage, homeRepository, toastr) {
            $scope.homeTabs = homeTabs;

            $scope.userId = undefined;
            $scope.suggestedInterests = [];
            $scope.suggestedInterestsLoaded = false;
            $scope.randomInterests = [];
            $scope.randomInterestsLoaded = false;
            $scope.selectedTab = homeTabs.SuggestedInterests;

            $scope.init = function () {
                $scope.userId = $localStorage.TPUserId;
                $scope.initSuggestedInterests();
            };

            $scope.setTab = function (tab) {
                $scope.selectedTab = tab;
            }

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