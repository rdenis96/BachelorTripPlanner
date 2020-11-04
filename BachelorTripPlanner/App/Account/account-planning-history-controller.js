globalModule.controller("PlanningHistoryController",
    ['$scope', '$localStorage', '$uibModal', 'accountRepository', 'toastr',
        function ($scope, $localStorage, $uibModal, accountRepository, toastr) {
            $scope.user = {};
            $scope.trips = {};

            $scope.init = function () {
                $scope.userId = $localStorage.TPUserId;
                var getUserTripsPromise = accountRepository.getUserTrips({ userId: $scope.userId }).$promise;
                getUserTripsPromise.then(function (result) {
                    $scope.trips = result;

                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.containsSearchText = function (trip) {
                if ($scope.searchText === null || $scope.searchText === undefined) {
                    return true;
                }
                return trip.name.toLowerCase().indexOf($scope.searchText.toLowerCase()) !== -1;
            };

            $scope.enableEdit = function (trip) {
                if (trip.editMode === undefined || trip.editMode === false) {
                    trip.editMode = true;
                }
                else {
                    trip.editMode = !trip.editMode;
                    //save changes
                }
            }

            // TO DO
            //$scope.deleteTrip = function (trip) {
            //    var getUserTripsPromise = accountRepository.deleteTrip({ userId: $scope.userId }).$promise;
            //    getUserTripsPromise.then(function (result) {
            //        $scope.trips = result;

            //    }).catch(function (result) {
            //        toastr.warning(result.data);
            //    });
            //}

            $scope.init();
        }

    ]);