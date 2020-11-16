globalModule.factory('notificationsRepository', [
    '$resource',
    function ($resource) {
        return $resource("api/notifications", {},
            {
                getNotifications: {
                    method: 'GET',
                    url: 'api/notifications/getNotifications',
                    isArray: true
                },
                sendNotification: {
                    method: 'POST',
                    url: 'api/notifications/sendNotification'
                },
                respondNotification: {
                    method: 'POST',
                    url: 'api/notifications/respondNotification'
                },
                deleteNotification: {
                    method: 'POST',
                    url: 'api/notifications/deleteNotification'
                }
            });
    }

]);