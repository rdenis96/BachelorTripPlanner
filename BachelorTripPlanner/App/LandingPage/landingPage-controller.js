globalModule.controller("LandingPageController",
    ['$scope', 'landingPageRepository',
        function ($scope, landingPageRepository) {
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
                var registerParamModel = {
                    email: $scope.registerEmail,
                    password: $scope.registerPassword,
                    ip: '127.0.0.1',
                    phone: '1242424'
                };

                var registerUserPromise = landingPageRepository.register(registerParamModel).$promise;
                registerUserPromise.then(function (result) {
                    alert("Register Successfull");
                });
            };
        }

    ]);