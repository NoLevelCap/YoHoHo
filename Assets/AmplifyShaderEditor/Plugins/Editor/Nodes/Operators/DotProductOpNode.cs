// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using System;
namespace AmplifyShaderEditor
{
	[Serializable]
	[NodeAttributes( "Dot", "Vector", "Scalar dot product of two vectors ( A . B )" )]
	public sealed class DotProductOpNode : DynamicTypeNode
	{
		protected override void CommonInit( int uniqueId )
		{
			base.CommonInit( uniqueId );
			m_outputPorts[ 0 ].ChangeType( WirePortDataType.FLOAT, false );
			m_dynamicOutputType = false;
			m_useInternalPortData = true;
			m_allowMatrixCheck = true;
			m_previewShaderGUID = "85f11fd5cb9bb954c8615a45c57a3784";
		}

		public override string BuildResults( int outputId, ref MasterNodeDataCollector dataCollector, bool ignoreLocalvar )
		{
			if ( m_outputPorts[ 0 ].IsLocalValue )
				return m_outputPorts[ 0 ].LocalValue;

			base.BuildResults( outputId, ref dataCollector, ignoreLocalvar );
			string result = "dot( " + m_inputA + " , " + m_inputB + " )";
			RegisterLocalVariable( 0, result, ref dataCollector, "dotResult" + OutputId );
			return m_outputPorts[ 0 ].LocalValue;
		}
	}
}
