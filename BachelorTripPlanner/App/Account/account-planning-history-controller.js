globalModule.controller("PlanningHistoryController",
    ['$scope', '$window', '$localStorage', '$uibModal', 'accountRepository', 'tripRepository', 'toastr',
        function ($scope, $window, $localStorage, $uibModal, accountRepository, tripRepository, toastr) {
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

            $scope.redirectToTrip = function (trip) { 
                if (trip.isDeleted === false) {
                    $window.location.href = '/trip/tripPlanner/' + trip.id;
                }
            };

            $scope.leaveTrip = function (tripId) {
                $scope.leaveTripPromise = tripRepository.leaveTrip({ userId: $scope.userId, tripId: tripId }).$promise;
                $scope.leaveTripPromise.then(function (result) {
                    $scope.init();
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            }

            $scope.init();
        }

    ]);