const requestType = 'PUSH/REQUEST_DATA';
const receiveType = 'PUSH/RECEIVE_DATA';
const initialState = { devices: [], isLoading: false };

export const actionCreators = {
    requestPushDevices: startDateIndex => async (dispatch, getState) => {
        if (startDateIndex === getState().push.startDateIndex) {
            // Don't issue a duplicate request (we already have or are loading the requested data)
            return;
        }

        dispatch({ type: requestType, startDateIndex });

        const url = `api/push/devices?startDateIndex=${startDateIndex}`;
        const response = await fetch(url);
        const devices = await response.json();

        dispatch({ type: receiveType, startDateIndex, devices });
    }
};

export const reducer = (state, action) => {
    state = state || initialState;

    if (action.type === requestType) {
        return {
            ...state,
            startDateIndex: action.startDateIndex,
            isLoading: true
        };
    }

    if (action.type === receiveType) {
        return {
            ...state,
            startDateIndex: action.startDateIndex,
            devices: action.devices,
            isLoading: false
        };
    }

    return state;
};
