globalModule.factory('homeRepository', [
    '$resource',
    function ($resource) {
        return $resource("api/home", {},
            {
                getSuggestedInterests: {
                    method: 'GET',
                    url: 'api/home/getSuggestedInterests',
                    isArray: true
                },
                getRandomInterests: {
                    method: 'GET',
                    url: 'api/home/getRandomInterests',
                    isArray: true
                }
            });
    }

]);