globalModule.factory('landingPageRepository', [
    '$resource',
    function ($resource) {
        return $resource("api/landingpage", {},
            {
                register: {
                    method: 'POST',
                    url: 'api/landingpage/register'
                },
                login: {
                    method: 'GET',
                    url: 'api/landingpage/login'
                }
            });
    }

]);