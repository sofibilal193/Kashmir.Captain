// using MediatR;
// using Microsoft.Extensions.Logging;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using Kashmir.Captain.Domain.Common;
// using Kashmir.Captain.Shared.Extensions;

// namespace Kashmir.Captain.Application.Behaviors
// {
//     public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//         where TRequest : IRequest<TResponse>
//     {
//         private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;
//         private readonly IDbContext? _dbContext;

//         public TransactionBehavior(IServiceProvider serviceProvider)
//         {
//             _logger = serviceProvider.GetRequiredService<ILogger<TransactionBehavior<TRequest, TResponse>>>();
//             var dbContexts = serviceProvider.GetServices<IDbContext>();
//             _dbContext = dbContexts.FirstOrDefault(c => c.SupportsTransactions)
//                 ?? dbContexts.FirstOrDefault();
//         }

//         public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
//             CancellationToken cancellationToken)
//         {
//             var response = default(TResponse);
//             var typeName = request.GetGenericTypeName();

//             try
//             {
//                 if (_dbContext is null)
//                 {
//                     return await next();
//                 }

//                 if (!_dbContext.SupportsTransactions)
//                 {
//                     return await next();
//                 }

//                 if (_dbContext.HasActiveTransaction)
//                 {
//                     return await next();
//                 }

//                 var strategy = _dbContext.Database.CreateExecutionStrategy();
//                 await strategy.ExecuteAsync(async () =>
//                 {
//                     Guid? transactionId = await _dbContext.BeginTransactionAsync(cancellationToken);
//                     _logger.LogInformation("----- Begin transaction {TransactionId} for {CommandName} ({@Command})", transactionId, typeName, request);
//                     response = await next();
//                     _logger.LogInformation("----- Commit transaction {TransactionId} for {CommandName}", transactionId, typeName);

//                     if (transactionId.HasValue)
//                     {
//                         await _dbContext.CommitTransactionAsync(cancellationToken);
//                     }
//                 });

//                 return response!;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);
//                 throw;
//             }
//         }
//     }
// }
