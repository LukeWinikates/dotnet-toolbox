var React = require('react');
var ReactDOM = require('react/lib/ReactDOM');

var PackageAdder = React.createClass({
  render() {
    return (
      <div>
        <button className="btn btn-default">+</button>
      </div>
    );
  }
});

ReactDOM.render(<PackageAdder />, document.getElementById('root'));