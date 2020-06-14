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
                getAvailableCities: {
                    method: 'GET',
                    url: 'api/account/GetAvailableCities',
                    isArray: true
                },
                getUserCitiesByUserCountriesAndAvailableCities: {
                    method: 'GET',
                    url: 'api/account/GetUserCitiesByUserCountriesAndAvailableCities'
                },
                getAvailableWeather: {
                    method: 'GET',
                    url: 'api/account/GetAvailableWeather',
                    isArray: true
                },
                getAvailableTransport: {
                    method: 'GET',
                    url: 'api/account/GetAvailableTransport',
                    isArray: true
                },
                getAvailableTouristAttractions: {
                    method: 'GET',
                    url: 'api/account/GetAvailableTouristAttractions',
                    isArray: true
                },
                update: {
                    method: 'PUT',
                    url: 'api/account/update'
                },
                updateCountriesAndCities: {
                    method: 'PUT',
                    url: 'api/account/updateInterestByCountryAndCity'
                },
                updateWeather: {
                    method: 'PUT',
                    url: 'api/account/updateInterestByWeather'
                },
                updateTransport: {
                    method: 'PUT',
                    url: 'api/account/updateInterestByTransport'
                },
                updateTouristAttractions: {
                    method: 'PUT',
                    url: 'api/account/updateInterestByTouristAttractions'
                }
            });
    }

]);