globalModule.factory('homeRepository', [
    '$resource',
    function ($resource) {
        return $resource("api/home",
            {
                getAll: {
                    method: 'GET',
                    url: 'api/home/getAll'
                }
            });
    }

]);