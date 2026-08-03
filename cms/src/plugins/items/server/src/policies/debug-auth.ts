export default (policyContext, config, { strapi }) => {
  console.log('=== AUTH DEBUG (admin router) ===');
  console.log('ctx.state.user:', policyContext.state.user);
  console.log('ctx.state.admin:', policyContext.state.admin);
  console.log('Cookies:', policyContext.request.header.cookie);
  console.log('==================================');
  return true;
};
