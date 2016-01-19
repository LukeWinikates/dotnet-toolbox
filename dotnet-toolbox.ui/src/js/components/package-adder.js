var React = require('react');
var $ = require('jquery');
import { SuccessAlert, ErrorAlert } from 'pui-react-alerts';
import { Link } from 'react-router'

function stateWith(newValues) {
  return Object.assign({pending: false, success: false, error: false}, newValues);
}

var PackageAdder = React.createClass({
  handleChange(event) {
    this.setState(stateWith({packageName: event.target.value}));
  },
  getInitialState() {
    return {}
  },
  hasPackageName() {
    return !!this.state.packageName;
  },
  submitPackage(e) {
    e.preventDefault();
    this.setState(stateWith({pending: true}));
    $.ajax({
      type: 'post',
      url: '/api/packages',
      data: JSON.stringify({name: this.state.packageName}),
      contentType: 'application/json'
    }).then(() => {
      this.setState(stateWith({success: true}));
    }, () =>{
      this.setState(stateWith({error: true}));
    });
  },
  render() {
    return (
      <div>
        <form className="form-inline" onSubmit={this.submitPackage}>
          <div className="h3">
            Help others by sharing Nuget packages that have helped you
          </div>
          <div className="form-group">
            <label className="sr-only" htmlFor="package-name">Package Name</label>
            <input className="form-control" id="package-name" readOnly={this.state.pending} placeholder="Nuget Package Name" type="text"
                   onChange={this.handleChange}
                   value={this.state.packageName}/>
          </div>
          <button className="btn btn-default" disabled={!this.hasPackageName() || this.state.pending} type="submit">
            {
              (() => {
                if (this.state.pending) {
                  return ( <marquee>...</marquee>);
                } else {
                  return (<span> add +</span>);
                }
              })()
            }
          </button>
        </form>
        {
          (() => {
            if(this.state.success) {
              return (<SuccessAlert dismissable>Package <Link to={"/package/" + this.state.packageName}>{this.state.packageName}</Link> Registered.</SuccessAlert>);
            }
          })()
        }
        {
          (() => {
            if(this.state.error) {
              return (<ErrorAlert dismissable>Unable to create package "{this.state.packageName}". This might be because the package does not exist at Nuget.org, or Nuget.org may be experiencing an outage.</ErrorAlert>);
            }
          })()
        }
      </div>
    );
  }
});

export default PackageAdder;