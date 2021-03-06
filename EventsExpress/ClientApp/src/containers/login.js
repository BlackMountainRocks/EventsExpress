﻿import { connect } from 'react-redux';
import React, { Component } from 'react';
import Login from '../components/login';
import login from '../actions/login';

class LoginWrapper extends Component {
  submit = async values => {
      return await this.props.login(values.email, values.password)
  };

  render() {
    return <Login onSubmit={this.submit}/>
  }
}

const mapStateToProps = state => {
  return {
    loginStatus: state.login
  }
};

const mapDispatchToProps = dispatch => {
  return {
    login: (email, password) => dispatch(login(email, password))
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(LoginWrapper);
