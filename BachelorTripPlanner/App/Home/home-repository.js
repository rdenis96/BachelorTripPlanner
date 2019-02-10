globalModule.factory('homeRepository', [
    '$resource',
    function ($resource) {
        return $resource("api/home",
            {
                'query': {
                    method: 'GET',
                    url: 'api/home/'
                }
            });
    }

]);