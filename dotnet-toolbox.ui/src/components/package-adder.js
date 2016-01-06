var React = require('react');
var ReactDOM = require('react/lib/ReactDOM');

var PackageAdder = React.createClass({
  render() {
    return (
      <div className="container up-xxxl panel paxxxl bg-neutral-11 panel-shadow">
        <form className="form-inline">
          <div className="h3">
            Help others by sharing Nuget packages that have helped you
          </div>
          <div className="form-group">
            <label className="sr-only" htmlFor="package-name">Package Name</label>
            <input className="form-control" id="package-name" placeholder="Nuget Package Name" type="text"/>
          </div>
          <button className="btn btn-default" type="submit">add +</button>
        </form>
      </div>
  );
  }
});

ReactDOM.render(<PackageAdder />, document.getElementById('root'));