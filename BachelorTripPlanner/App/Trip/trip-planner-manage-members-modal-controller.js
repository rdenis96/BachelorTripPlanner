globalModule.controller("TripPlannerManageMembersModalController",
    ['$scope', 'data', '$window', '$http', '$localStorage', 'tripRepository', 'toastr', '$routeParams', '$uibModal',
        function ($scope, data, $window, $http, $localStorage, tripRepository, toastr, $routeParams, $uibModal) {
            $scope.userId = data.userId;
            $scope.tripId = data.tripId;
            $scope.isAdmin = data.isAdmin;

            $scope.members = [];

            $scope.init = function () {
                $scope.getTripUsersPromise = tripRepository.getTripUsers({ userId: $scope.userId }).$promise;
                $scope.getTripUsersPromise.then(function (result) {
                    $scope.members = result;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            }
        }

    ]);