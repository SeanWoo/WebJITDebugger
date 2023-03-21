module.exports = {
  '/api': {
    target: 'http://localhost:5000',
    pathRewrite: { '^/api': '' },
    changeOrigin: true,
  },
  '/swagger': {
    target: 'http://localhost:5000',
    changeOrigin: true,
  },
};
