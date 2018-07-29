import React, { Component } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { actionCreators } from '../store/Push';

class Push extends Component {
    componentWillMount() {
        // This method runs when the component is first added to the page
        const startDateIndex = parseInt(this.props.match.params.startDateIndex, 10) || 0;
        this.props.requestPushDevices(startDateIndex);
    }

    componentWillReceiveProps(nextProps) {
        // This method runs when incoming props (e.g., route params) change
        const startDateIndex = parseInt(nextProps.match.params.startDateIndex, 10) || 0;
        this.props.requestPushDevices(startDateIndex > 0 && startDateIndex || 0);
    }

    render() {
        return (
            <div>
                <h1>Список устройств</h1>
                <p>Все устройства подписанные на уведомления</p>
                {renderTable(this.props)}
                {renderPagination(this.props)}
            </div>
        );
    }
}

function renderTable(props) {
    const onClick = async device => { 
        const request = await fetch("/api/push/notify", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
            },
            body: JSON.stringify({
                id: device.id,
                withPause: Math.random() > 0.5
            })
        });
    };
    return (
        <table className='table'>
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Endpoint</th>
                    <th>P256dh</th>
                    <th>Auth</th>
                </tr>
            </thead>
            <tbody>
                {props.devices.map(device =>
                    <tr key={device.id} onClick={_ => onClick(device)}>
                        <td>{device.id}</td>
                        <td>{device.endpoint}</td>
                        <td>{device.p256dh}</td>
                        <td>{device.auth}</td>
                    </tr>
                )}
            </tbody>
        </table>
    );
}

function renderPagination(props) {
    const startDateIndex = props.startDateIndex > 0 && props.startDateIndex || 0
    const prevStartDateIndex = startDateIndex - 1
    const nextStartDateIndex = startDateIndex + 1
    return <p className='clearfix text-center'>
        {startDateIndex > 0 ? <Link className='btn btn-default pull-left' to={`/push/${prevStartDateIndex}`}>Previous</Link> : []}
        <Link className='btn btn-default pull-right' to={`/push/${nextStartDateIndex}`}>Next</Link>
        {props.isLoading ? <span>Loading...</span> : []}
    </p>;
}

export default connect(
    state => state.push,
    dispatch => bindActionCreators(actionCreators, dispatch)
)(Push);
