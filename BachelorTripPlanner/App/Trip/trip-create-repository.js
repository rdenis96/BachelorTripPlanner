globalModule.factory('tripCreateRepository', [
    '$resource',
    function ($resource) {
        return $resource("api/trip", {},
            {
                createTrip: {
                    method: 'POST',
                    url: 'api/trip/createTrip'
                }
            });
    }

]);