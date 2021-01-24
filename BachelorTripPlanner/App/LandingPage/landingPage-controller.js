globalModule.controller("LandingPageController",
    ['$scope', '$window', '$http', '$localStorage', 'landingPageRepository', 'toastr',
        function ($scope, $window, $http, $localStorage, landingPageRepository, toastr) {
            $scope.landingPage = true;
            $scope.landingPageTabsEnum = landingPageTabsEnum;
            $scope.selectedTab = landingPageTabsEnum.Welcome;

            $scope.changeTab = function (tab) {
                switch (tab) {
                    case landingPageTabsEnum.Welcome: {
                        $scope.selectedTab = landingPageTabsEnum.Welcome;
                        break;
                    }
                    case landingPageTabsEnum.Login: {
                        $scope.selectedTab = landingPageTabsEnum.Login;
                        break;
                    }
                    case landingPageTabsEnum.Register: {
                        $scope.selectedTab = landingPageTabsEnum.Register;
                        break;
                    }
                }
            };

            $scope.register = function () {
                $http.get('https://ipapi.co/json/').success(function (response) {
                    $scope.registerIp = response.ip;

                    var registerParamModel = {
                        email: $scope.registerEmail,
                        password: $scope.registerPassword,
                        registerDate: new Date(),
                        lastOnline: null,
                        ip: $scope.registerIp,
                        phone: $scope.registerPhoneNumber != undefined && $scope.registerPhoneNumber.length > 0 ? $scope.registerPhoneNumber : null
                    };

                    var registerUserPromise = landingPageRepository.register(registerParamModel).$promise;
                    registerUserPromise.then(function (result) {
                        toastr.success(result.message);
                        $scope.changeTab(landingPageTabsEnum.Login);
                    }).catch(function (result) {
                        toastr.warning(result.data);
                    });
                });
            };

            $scope.login = function () {
                var loginParamModel = {
                    email: $scope.loginEmail,
                    password: $scope.loginPassword
                };

                var loginUserPromise = landingPageRepository.login(loginParamModel).$promise;
                loginUserPromise.then(function (result) {
                    $window.location.href = '/home';
                    toastr.success(result.message);
                }).catch(function (result) {
                    console.log(result);
                    toastr.warning(result.data);
                });
            };
        }

    ]);