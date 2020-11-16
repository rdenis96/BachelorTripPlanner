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
                },
                getTripUsers: {
                    method: 'GET',
                    url: 'api/trip/getTripUsers',
                    isArray: true
                },
                updateTripUsers: {
                    method: 'POST',
                    url: 'api/trip/updateTripUsers',
                    isArray: true
                },
                addNewTripMember: {
                    method: 'GET',
                    url: 'api/trip/addNewTripMember',
                    isArray: true
                },
                removeUserFromTrip: {
                    method: 'GET',
                    url: 'api/trip/removeUserFromTrip'
                },
                resetUserInterests: {
                    method: 'GET',
                    url: 'api/trip/resetUserInterests'
                },
                leaveTrip: {
                    method: 'GET',
                    url: 'api/trip/leaveTrip'
                },
                getTrip: {
                    method: 'GET',
                    url: 'api/trip/getTrip'
                }
            });
    }

]);