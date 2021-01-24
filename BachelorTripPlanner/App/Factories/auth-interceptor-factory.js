globalModule.factory('authInterceptor', [
    '$rootScope', '$q', '$location', '$window', '$timeout', '$localStorage',
    function ($rootScope, $q, $location, $window, $timeout, $localStorage) {
        return {
            request: function (config) {
                if ($localStorage.AccessToken !== undefined && $localStorage.TPUserId !== undefined) {
                    config.headers.Authorization = "Bearer " + $localStorage.AccessToken;
                }

                return config || $q.when(config);
            },
            response: function (response) {
                if (response) {
                    if (response.config.url.toLowerCase() === "api/landingpage/login") {
                        $localStorage.AccessToken = response.data.token;
                        $localStorage.TPUserId = response.data.userId;
                    }
                }

                return response;
            },
            responseError: function (response) {
                if (response) {
                    //unauthorized || forbidden - session expired
                    if (response.status === httpStatusCodeEnum.Unauthorized || response.status === httpStatusCodeEnum.Forbidden) {
                        $window.location.href = '/welcome';
                    }
                }

                return $q.reject(response);
            }
        }
    }
]);