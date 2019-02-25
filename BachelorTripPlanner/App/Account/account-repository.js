globalModule.factory('accountRepository', [
    '$resource',
    function ($resource) {
        return $resource("api/account", {},
            {
                getUser: {
                    method: 'GET',
                    url: 'api/account/getUser'
                },
                update: {
                    method: 'PUT',
                    url: 'api/account/update'
                }
            });
    }

]);