globalModule.factory('accountRepository', [
    '$resource',
    function ($resource) {
        return $resource("api/account", {},
            {
                getUser: {
                    method: 'GET',
                    url: 'api/account/getUser'
                },
                getUserInterest: {
                    method: 'GET',
                    url: 'api/account/getUserInterest'
                },
                getAvailableCountries: {
                    method: 'GET',
                    url: 'api/account/GetAvailableCountries',
                    isArray: true
                },
                update: {
                    method: 'PUT',
                    url: 'api/account/update'
                },
                updateCountriesAndCities: {
                    method: 'PUT',
                    url: 'api/account/updateCountriesAndCities'
                }
            });
    }

]);