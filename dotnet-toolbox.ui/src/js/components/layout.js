var React = require('react');

var LeaderPanel = React.createClass({
  render() {
    return (
      <div className="container up-xxxl panel paxxxl bg-neutral-11 panel-shadow">
        {this.props.children}
      </div>
    )
  }
});

export default {LeaderPanel};