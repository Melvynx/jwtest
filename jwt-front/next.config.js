/**
 * @type {import('next').NextConfig}
 */
const nextConfig = {
  /* config options here */
  env: {
    JWT_API_URL: process.env.JWT_API_URL,
    APP_API_URL: process.env.APP_API_URL,
  },
};

module.exports = nextConfig;
