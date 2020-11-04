globalModule.factory('tripRepository', [
    '$resource',
    function ($resource) {
        return $resource("api/trip", {},
            {
                getUserHeaderTrips: {
                    method: 'GET',
                    url: 'api/trip/getUserHeaderTrips',
                    isArray: true
                },
                getUserInterestForTrip: {
                    method: 'GET',
                    url: 'api/trip/getUserInterestForTrip'
                },
                getInterestsForTrip: {
                    method: 'GET',
                    url: 'api/trip/getSuggestedInterests',
                    isArray: true
                },
                getMessagesForTrip: {
                    method: 'GET',
                    url: 'api/trip/getMessages',
                    isArray: true
                },
                createMessage: {
                    method: 'POST',
                    url: 'api/trip/createMessage'
                },
                isUserAdmin: {
                    method: 'GET',
                    url: 'api/trip/isUserAdmin'
                }
            });
    }

]);