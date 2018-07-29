import { appServerKeyNotification } from '../../config'
export default class Notifications {
    constructor(sw) { 
        if ('serviceWorker' in navigator && 'PushManager' in window) {
            this.sw = sw; 
        }
    }

    async subscribeUser() {
        try {
            const subscription = await this.sw.pushManager.subscribe({
                userVisibleOnly: true,
                applicationServerKey: urlB64ToUint8Array(appServerKeyNotification)
            });

            try {
                const response = await fetch('/api/push/subscribe', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(subscription)
                })
            } catch (e) {
                console.error('error fetching subscribe', e);
            }
        } catch (err) {
            console.log('Failed to subscribe the user: ', err);
        }
    }

    async unsubscribeUser() {
        try {
            const subscription = await this.sw.pushManager.getSubscription();
            if (subscription) {
                const subscriptionData = {
                    endpoint: subscription.endpoint
                };
                try {
                    const response = await fetch('/api/push/unsubscribe', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify(subscriptionData)
                    });
                } catch (err) {
                    console.error('error fetching unsubscribe', err);
                }

                return subscription.unsubscribe()
            } else {
                console.log('subscription is no exsits');
            }
        }
        catch (e) {
            console.log('error get subscription');
        }
    }
}


function urlB64ToUint8Array(base64String) {
    const padding = '='.repeat((4 - base64String.length % 4) % 4);
    const base64 = (base64String + padding)
        .replace(/\-/g, '+')
        .replace(/_/g, '/');

    const rawData = window.atob(base64);
    const outputArray = new Uint8Array(rawData.length);

    for (let i = 0; i < rawData.length; ++i) {
        outputArray[i] = rawData.charCodeAt(i);
    }
    return outputArray;
}
