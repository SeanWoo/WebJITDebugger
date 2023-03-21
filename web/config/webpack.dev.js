const { merge } = require('webpack-merge');

const paths = require('./paths');
const proxy = require('./proxy');
const commonConfig = require('./webpack.common');

module.exports = merge(commonConfig, {
  mode: 'development',
  target: 'web',
  devtool: 'eval-source-map',
  stats: 'errors-warnings',
  devServer: {
    port: 3000,
    open: true,
    hot: true,
    compress: true,
    historyApiFallback: true,
    proxy,
    client: {
      overlay: { errors: true, warnings: false },
      logging: 'error',
      progress: true,
    },
    static: [
      {
        directory: paths.public,
      },
    ],
  },
  cache: {
    type: 'filesystem',
    allowCollectingMemory: true,
  },
  module: {
    rules: [
      {
        test: /\.(ts|tsx)?$/,
        exclude: /node_modules/,
        use: [
          {
            loader: 'thread-loader',
            options: {
              poolTimeout: 2000,
            },
          },
          'babel-loader',
        ],
      },
    ],
  },
  plugins: [],
});
